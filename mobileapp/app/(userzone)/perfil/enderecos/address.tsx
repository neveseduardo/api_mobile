import { useState } from 'react';
import { View, Text } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import TextInput from '@/components/ui/TextInput';
import Button from '@/components/ui/Button';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import Ionicons from '@expo/vector-icons/Ionicons';
import { HttpClient } from '@/services/HttpClient';
import { UserAddressService } from '@/services/userservices/UserAddressService';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/UserAuthenticationContext';

const loginSchema = z.object({
	cep: z.string().min(1, 'Campo obrigatório!!'),
	logradouro: z.string().min(1, 'Campo obrigatório!!'),
	numero: z.string().min(0),
	complemento: z.string().min(0),
	bairro: z.string().min(1, 'Campo obrigatório!!'),
	cidade: z.string().min(1, 'Campo obrigatório!!'),
	estado: z.string().min(1, 'Campo obrigatório!!'),
});

type LoginFormData = z.infer<typeof loginSchema>;

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UserAddressService(client);

export default function AddressScreen() {
	const router = useRouter();
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			cep: (routeParams.cep ?? '') as string,
			logradouro: (routeParams.logradouro ?? '') as string,
			numero: (routeParams.numero) as string,
			complemento: (routeParams.complemento ?? '') as string,
			bairro: (routeParams.bairro ?? '') as string,
			cidade: ((routeParams.localidade ?? routeParams.cidade) ?? '') as string,
			estado: (routeParams.estado ?? '') as string,
		},
	});

	const [loading, setLoading] = useState(false);

	const onSubmit = async (data: LoginFormData) => {
		setLoading(true);

		try {
			const payload = {
				...data,
				cep: String(routeParams.cep).replace(/(\d{5})(\d{3})/, '$1-$2'),
			};

			if (!routeParams.editable) {
				await service.addFromUserAsync(payload);
			}

			if (routeParams.editable) {
				await service.updateFromUserAsync(Number(routeParams.id), payload);
			}
			router.push('/(userzone)/perfil/enderecos');
		} catch (error: any) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					icon="map-outline"
					title="Casdastrar endereço"
					description="Insira os dados solicitados abaixo para cadastrar seu endereço."
				/>

				<View className="flex flex-col w-full gap-4">
					<Controller
						control={control}
						name="cep"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="CEP"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={true}
								error={!!errors.cep}
								errorMessage={errors.cep?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="logradouro"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Logradouro"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.logradouro}
								errorMessage={errors.logradouro?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="numero"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								keyboardType="numeric"
								returnKeyType="next"
								placeholder="Número"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.numero}
								errorMessage={errors.numero?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="complemento"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Complemento"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.complemento}
								errorMessage={errors.complemento?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="bairro"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Bairro"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={true}
								error={!!errors.bairro}
								errorMessage={errors.bairro?.message}
							/>
						)}
					/>

					<View className="grid grid-cols-2 gap-4">
						<Controller
							control={control}
							name="cidade"
							render={({ field: { onChange, onBlur, value } }) => (
								<TextInput
									returnKeyType="next"
									placeholder="Cidade"
									value={value}
									onChangeText={onChange}
									onBlur={onBlur}
									disabled={true}
									error={!!errors.cidade}
									errorMessage={errors.cidade?.message}
								/>
							)}
						/>
						<Controller
							control={control}
							name="estado"
							render={({ field: { onChange, onBlur, value } }) => (
								<TextInput
									returnKeyType="next"
									placeholder="Estado"
									value={value}
									onChangeText={onChange}
									onBlur={onBlur}
									disabled={true}
									error={!!errors.estado}
									errorMessage={errors.estado?.message}
								/>
							)}
						/>
					</View>

					<Button
						onPress={handleSubmit(onSubmit)}
						color="primary"
						className="flex flex-row justify-center w-full gap-2"
						disabled={loading}
					>
						<Text className="text-white">
							{routeParams.editable ? 'ATUALIZAR' : 'CADASTRAR'}
						</Text>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}