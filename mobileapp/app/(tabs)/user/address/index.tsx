import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';

const UserScreen = () => {
	const router = useRouter();

	return (
		<ThemedView className="items-center justify-center flex-1 p-5">
			<ThemedView className="flex items-center justify-center w-full gap-4">
				<ThemedText>LISTA DE ENDEREÇOS</ThemedText>

				<Button onPress={() => router.push('/(tabs)/user/address/postalcode')} className="w-full">
					<ThemedText>CADASTRAR ENDEREÇO</ThemedText>
				</Button>
			</ThemedView>
		</ThemedView>
	);
};

export default UserScreen;