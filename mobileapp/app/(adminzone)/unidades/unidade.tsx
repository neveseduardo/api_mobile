import { useState } from 'react';
import { View, Text } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { ThemedView } from '@/components/ui/ThemedView';
import TextInput from '@/components/ui/TextInput';
import Button from '@/components/ui/Button';
import AuthHeader from '@/components/modules/auth/AuthHeader';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { HttpClient } from '@/services/HttpClient';
import { USER_ACCESS_TOKEN_NAME } from '@/contexts/AdminAuthenticationContext';
import { UnitService } from '@/services/UnitService';
import { z } from 'zod';
import Ionicons from '@expo/vector-icons/Ionicons';

const loginSchema = z.object({
	name: z.string().min(1, 'Campo obrigatório!!'),
	phoneNumber: z.string().min(1, 'Campo obrigatório!!'),
	email: z.string().min(1, 'Email é obrigatório').email('Email inválido'),
});

type LoginFormData = z.infer<typeof loginSchema>;

const { client } = HttpClient(USER_ACCESS_TOKEN_NAME);
const service = new UnitService(client);

export default function UnidadeScreen() {
	const router = useRouter();
	const params = useLocalSearchParams();
	const { ...routeParams } = params;

	const { control, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
		resolver: zodResolver(loginSchema),
		defaultValues: {
			name: (routeParams.name ?? '') as string,
			phoneNumber: (routeParams.phoneNumber ?? '') as string,
			email: (routeParams.email) as string,
		},
	});

	const [loading, setLoading] = useState(false);

	const onSubmit = async (data: LoginFormData) => {
		setLoading(true);

		try {
			const payload = {
				...data,
				cep: String(routeParams.cep).replace(/(\d{5})(\d{3})/, '$1-$2'),
				logradouro: routeParams.logradouro,
				numero: routeParams.numero,
				complemento: routeParams.complemento,
				bairro: routeParams.bairro,
				cidade: routeParams.cidade,
				estado: routeParams.estado,
			};

			if (!routeParams.editable) {
				await service.addFromAdminAsync(payload);
			}

			if (routeParams.editable) {
				await service.updateFromAdminAsync(Number(routeParams.id), payload);
			}
			router.push('/(adminzone)/unidades');
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
						name="name"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="next"
								placeholder="Nome"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.name}
								errorMessage={errors.name?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="email"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								keyboardType="email-address"
								returnKeyType="next"
								placeholder="Email"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.email}
								errorMessage={errors.email?.message}
							/>
						)}
					/>

					<Controller
						control={control}
						name="phoneNumber"
						render={({ field: { onChange, onBlur, value } }) => (
							<TextInput
								returnKeyType="done"
								placeholder="Telefone"
								value={value}
								onChangeText={onChange}
								onBlur={onBlur}
								disabled={loading}
								error={!!errors.phoneNumber}
								errorMessage={errors.phoneNumber?.message}
							/>
						)}
					/>

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